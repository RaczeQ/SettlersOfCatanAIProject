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
        }

        public void executeUpdate(Object sender, EventArgs e)
        {
            switch (currentState)
            {
                case State.Wait:
                    //Waiting for user input.
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
                            ((Settlement)sender).buildSettlement(theBoard.currentPlayer, true);
                            //Take resources
                            if (location.city())
                            {
                                //take city resources
                                Board.TheBank.takePayment(theBoard.currentPlayer, Bank.CITY_COST);
                                theBoard.addEventText(UserMessages.PlayerPlacedASettlement(theBoard.currentPlayer));
                            } else
                            {
                                //take settlement resources
                                Board.TheBank.takePayment(theBoard.currentPlayer, Bank.SETTLEMENT_COST);
                                theBoard.addEventText(UserMessages.PlayerBuiltACity(theBoard.currentPlayer));
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
                        } catch (BuildError be)
                        {
                            theBoard.addEventText(be.Message);
                        }
                    } else if (sender == theBoard.pbBuildDevelopmentCard)
                    {
                        //We want to give the player a development card.
                        try
                        {
                            theBoard.currentPlayer.giveDevelopmentCard(Board.TheBank.purchaseDevelopmentCard(theBoard.currentPlayer));
                            Board.TheBank.takePayment(theBoard.currentPlayer, Bank.DEV_CARD_COST);
                            theBoard.addEventText(UserMessages.PlayerPurchasedADevCard(theBoard.currentPlayer));
                        } catch (BuildError be)
                        {
                            theBoard.addEventText(be.Message);
                        }
                    } else if (sender is DevelopmentCard)
                    {
                        DevelopmentCard card = (DevelopmentCard)sender;
                        switch (card.getType())
                        {
                            case DevelopmentCard.DevCardType.Road:
                                RoadBuildingEvt evt = new RoadBuildingEvt();
                                evt.beginExecution(theBoard, this);
                                disableEventObjects();
                                Board.TheBank.developmentCards.putCardBottom(theBoard.currentPlayer.takeDevelopmentCard(card.getType()));
                                break;
                            case DevelopmentCard.DevCardType.Plenty:
                                YearOfPlentyEvt yr = new YearOfPlentyEvt();
                                yr.beginExecution(theBoard, this);
                                disableEventObjects();
                                Board.TheBank.developmentCards.putCardBottom(theBoard.currentPlayer.takeDevelopmentCard(card.getType()));
                                break;
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
            disableEventObjects();
            tradeWindow = new TradeWindow();
            tradeWindow.loadPlayerTrade(theBoard.currentPlayer);
            tradeWindow.Closing += onTradeEnded;
            currentState = State.Trade;
            tradeWindow.Show();
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
            theBoard.pnlDevelopmentCardToolTip.Enabled = false;
            theBoard.pnlRoadToolTip.Enabled = false;
            theBoard.pnlSettlementToolTip.Enabled = false;
            theBoard.disableToolTips();
        }
    }
}
