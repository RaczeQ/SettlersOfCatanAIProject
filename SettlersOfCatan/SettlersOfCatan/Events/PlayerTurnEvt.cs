using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlersOfCatan.Events
{
    class PlayerTurnEvt : Event, EvtOwnr
    {

        enum State { Trade, Build, Purchase, Development, Wait };
        State currentState = State.Wait;
        Board theBoard;
        TradeWindow tradeWindow;

        public void beginExecution(Board board, EvtOwnr evt)
        {
            theBoard = board;
            enableEventObjects();

            foreach (DevelopmentCard devC in board.currentPlayer.getDevelopmentCards())
            {
                devC.setPlayable(true);
            }

        }

        public void executeUpdate(Object sender, EventArgs e)
        {
            switch (currentState)
            {
                case State.Wait:
                    if (sender == theBoard.btnEndTurn)
                    {
                        //End turn.
                        theBoard.addEventText(UserMessages.PlayerEndedTurn(theBoard.currentPlayer));
                        endExecution();
                    } else if (sender is Settlement)
                    {
                        Settlement location = (Settlement)sender;
                        try
                        {
                            //Try to build the settlement/upgrade city.
                            ((Settlement)sender).buildSettlement(theBoard.currentPlayer, true, true);
                            //Take resources
                            if (location.city())
                            {
                                //take city resources
                                Board.TheBank.takePayment(theBoard.currentPlayer, Bank.CITY_COST);
                                theBoard.addEventText(UserMessages.PlayerBuiltACity(theBoard.currentPlayer));
                                theBoard.checkForWinner();
                            } else
                            {
                                //take settlement resources
                                Board.TheBank.takePayment(theBoard.currentPlayer, Bank.SETTLEMENT_COST);
                                theBoard.addEventText(UserMessages.PlayerPlacedASettlement(theBoard.currentPlayer));
                                theBoard.checkForWinner();
                            }
                        } catch (BuildError be)
                        {
                            theBoard.addEventText(be.Message);
                        }

                    } else if (sender is Road)
                    {
                        //Try to build the road
                        try
                        {
                            ((Road)sender).buildRoad(theBoard.currentPlayer, true);
                            //Take the resources.
                            Board.TheBank.takePayment(theBoard.currentPlayer, Bank.ROAD_COST);
                            theBoard.addEventText(UserMessages.PlayerPlacedARoad(theBoard.currentPlayer));
                            //RUN LONGEST ROAD CHECK
                            theBoard.checkForWinner();
                        } catch (BuildError be)
                        {
                            theBoard.addEventText(be.Message);
                        }
                    } else if (sender == theBoard.pbBuildDevelopmentCard)
                    {
                        //We want to give the player a development card.
                        try
                        {
                            DevelopmentCard devC = Board.TheBank.purchaseDevelopmentCard(theBoard.currentPlayer);
                            devC.setPlayable(false);
                            theBoard.currentPlayer.giveDevelopmentCard(devC);
                            Board.TheBank.takePayment(theBoard.currentPlayer, Bank.DEV_CARD_COST);
                            theBoard.addEventText(UserMessages.PlayerPurchasedADevCard(theBoard.currentPlayer));

                            theBoard.checkForWinner();
                        } catch (BuildError be)
                        {
                            theBoard.addEventText(be.Message);
                        }
                    } else if (sender is DevelopmentCard)
                    {
                        DevelopmentCard card = (DevelopmentCard)sender;
                        if (card.isPlayable())
                        {
                            switch (card.getType())
                            {
                                case DevelopmentCard.DevCardType.Road:
                                    //Player is allowed to place two roads for no cost
                                    RoadBuildingEvt evt = new RoadBuildingEvt();
                                    evt.beginExecution(theBoard, this);
                                    disableEventObjects();
                                    //Remove this card from the player's hand.
                                    theBoard.currentPlayer.takeDevelopmentCard(card.getType());
                                    break;
                                case DevelopmentCard.DevCardType.Plenty:
                                    //Player is allowed to take any two resources from the bank
                                    YearOfPlentyEvt yr = new YearOfPlentyEvt();
                                    yr.beginExecution(theBoard, this);
                                    disableEventObjects();
                                    //Remove this card from the player's hand. They DO NOT go back to the bank
                                    theBoard.currentPlayer.takeDevelopmentCard(card.getType());
                                    break;
                                case DevelopmentCard.DevCardType.Knight:
                                    //Launch the thief event
                                    if (!card.used)
                                    {
                                        ThiefEvt thevt = new ThiefEvt();
                                        thevt.beginExecution(theBoard, this);
                                        disableEventObjects();
                                        card.used = true;
                                        theBoard.checkForWinner();
                                    }
                                    break;
                                case DevelopmentCard.DevCardType.Monopoly:
                                    //Player names a resource, all players give this player that resource they carry
                                    MonopolyEvt monEvt = new MonopolyEvt();
                                    monEvt.beginExecution(theBoard, this);
                                    disableEventObjects();
                                    //Remove this card from the player's hand. They DO NOT go back to the bank
                                    theBoard.currentPlayer.takeDevelopmentCard(card.getType());
                                    break;
                            }
                        }
                        else
                        {
                            if (card.getType() != DevelopmentCard.DevCardType.Victory)
                            {
                                theBoard.addEventText("You must wait until the next turn to play that card.");
                            }
                        }
                    } else if (sender is Harbor)
                    {
                        Harbor hb = (Harbor)sender;
                        if (hb.playerHasValidSettlement(theBoard.currentPlayer))
                        {
                            if (TradeWindow.canPlayerTradeWithHarbor(hb, theBoard.currentPlayer))
                            {
                                disableEventObjects();
                                tradeWindow = new TradeWindow();
                                tradeWindow.loadHarborTrade(hb, theBoard.currentPlayer);
                                tradeWindow.Closing += onTradeEnded;
                                currentState = State.Trade;
                                tradeWindow.Show();
                            } else
                            {
                                theBoard.addEventText(BuildError.NOT_ENOUGH_RESOURCES);
                            }
                        } else
                        {
                            theBoard.addEventText(UserMessages.PlayerDoesNotHaveAdjascentSettlement);
                        }
                    }
                    break;
                case State.Trade:
                    //Should not be able to reach this statement.
                    break;

            }
        }

        public void bankTrade(Object sender, EventArgs e)
        {
            disableEventObjects();
            tradeWindow = new TradeWindow();
            tradeWindow.loadBankTrade(theBoard.currentPlayer);
            tradeWindow.Closing += onTradeEnded;
            currentState = State.Trade;
            tradeWindow.Show();
        }
        
        public void playerTrade(Object sender, EventArgs e)
        {
            if (sender is Player)
            {
                disableEventObjects();
                tradeWindow = new TradeWindow();
                tradeWindow.loadPlayerTrade(theBoard.currentPlayer, (Player)sender);
                tradeWindow.Closing += onTradeEnded;
                currentState = State.Trade;
                tradeWindow.Show();
            }
        }

        public void onTradeEnded(Object sender, EventArgs e)
        {
            //The trade ended.
            enableEventObjects();
            tradeWindow = null;
            currentState = State.Wait;
        }

        public void subeventEnded()
        {
            enableEventObjects();
        }

        public void endExecution()
        {
            disableEventObjects();
            theBoard.subeventEnded();
        }

        public void enableEventObjects()
        {
            theBoard.btnBankTrade.Enabled = true;
            theBoard.btnPlayerTrade.Enabled = true;
            theBoard.btnEndTurn.Enabled = true;
            theBoard.btnBankTrade.Click += bankTrade;
            theBoard.btnPlayerTrade.Click += playerTrade;
            theBoard.btnEndTurn.Click += executeUpdate;
            theBoard.pbBuildDevelopmentCard.Click += executeUpdate;
            foreach (Road rd in theBoard.roadLocations)
            {
                rd.Click += executeUpdate;
            }
            foreach (Settlement st in theBoard.settlementLocations)
            {
                st.Click += executeUpdate;
            }
            foreach (Harbor hb in theBoard.harbors)
            {
                hb.Click += executeUpdate;
            }
            foreach (DevelopmentCard devc in theBoard.currentPlayer.getDevelopmentCards())
            {
                devc.Click += executeUpdate;
            }
            foreach (Player p in theBoard.playerOrder)
            {
                if (p != theBoard.currentPlayer)
                {
                    p.Click += playerTrade;
                }
            }
            theBoard.enableToolTips();
        }

        public void disableEventObjects()
        {
            theBoard.btnBankTrade.Enabled = false;
            theBoard.btnPlayerTrade.Enabled = false;
            theBoard.btnEndTurn.Enabled = false;
            theBoard.btnBankTrade.Click -= bankTrade;
            theBoard.btnPlayerTrade.Click -= playerTrade;
            theBoard.btnEndTurn.Click -= executeUpdate;
            theBoard.pbBuildDevelopmentCard.Click -= executeUpdate;
            foreach (Road rd in theBoard.roadLocations)
            {
                rd.Click -= executeUpdate;
            }
            foreach (Settlement st in theBoard.settlementLocations)
            {
                st.Click -= executeUpdate;
            }
            foreach (Harbor hb in theBoard.harbors)
            {
                hb.Click -= executeUpdate;
            }
            foreach (DevelopmentCard devc in theBoard.currentPlayer.getDevelopmentCards())
            {
                devc.Click -= executeUpdate;
            }
            foreach (Player p in theBoard.playerOrder)
            {
                if (p != theBoard.currentPlayer)
                {
                    p.Click -= playerTrade;
                }
            }
            theBoard.pnlDevelopmentCardToolTip.Enabled = false;
            theBoard.pnlRoadToolTip.Enabled = false;
            theBoard.pnlSettlementToolTip.Enabled = false;
            theBoard.disableToolTips();
        }
    }
}
