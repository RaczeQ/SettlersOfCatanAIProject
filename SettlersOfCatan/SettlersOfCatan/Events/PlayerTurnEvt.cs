using SettlersOfCatan.AI;
using SettlersOfCatan.AI.AssesmetFunctions;
using SettlersOfCatan.Events;
using SettlersOfCatan.GameObjects;
using SettlersOfCatan.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersOfCatan.Moves;

namespace SettlersOfCatan.Eventss
{
    class PlayerTurnEvt : Event, EvtOwnr
    {

        enum State { Trade, Build, Purchase, Development, Wait };
        State currentState = State.Wait;
        Board theBoard;
        TradeWindow tradeWindow;

        public override void beginExecution(Board board, EvtOwnr evt)
        {
            theBoard = board;

            foreach (DevelopmentCard devC in board.currentPlayer.getDevelopmentCards())
            {
                devC.setPlayable(true);
            }
            
            if (!theBoard.currentPlayer.isAI)
            {
                enableEventObjects();
            }
            else
            {
                while (PlayerSemaphore.isLocked)
                {
                    var move = theBoard.currentPlayer.agent.makeMove(theBoard.getBoardState());
                    executeUpdate(move, null);
                    System.Threading.Thread.Sleep(Board.AITurnDelayMilliseconds);
                }
            }
        }

        public override void executeUpdate(Object sender, EventArgs e)
        {
            switch (currentState)
            {
                case State.Wait:
                    if (sender == theBoard.btnEndTurn || sender is EndMove)
                    {
                        //End turn.
                        theBoard.addEventText(UserMessages.PlayerEndedTurn(theBoard.currentPlayer));
                        endExecution();
                        return;
                    } else if (sender is Settlement)
                    {
                        var location = theBoard.settlementLocations.Find(s => Equals(s, sender));
                        if (location.owningPlayer != null)
                        {
                            makeMove(new BuildCityMove((Settlement)sender));
                        }
                        else
                        {
                            makeMove(new BuildSettlementMove((Settlement)sender));
                        }

                    } else if (sender is Road)
                    {
                        makeMove(new BuildRoadMove((Road) sender));
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

                            theBoard.CheckForWinner();
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
                                        theBoard.CheckForWinner();
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
                    } else if (sender is Move)
                    {
                        makeMove((Move) sender);
                    }
                    break;
                case State.Trade:
                    //Should not be able to reach this statement.
                    break;

            }
            //if (theBoard.currentPlayer.isAI)
            //{ 
            //    var move = theBoard.currentPlayer.agent.makeMove(theBoard.getBoardState());
            //    executeUpdate(move, null);
            //}
        }

        private void makeMove(Move move)
        {
            try
            {
                move.MakeMove(ref theBoard);
                theBoard.CheckForWinner();
            }
            catch (Exception e)
            {
                theBoard.addEventText(e.Message);
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

        public bool subeventEnded()
        {
            enableEventObjects();
            return true;
        }

        public override void endExecution()
        {
            disableEventObjects();
            //theBoard.subeventEnded();
            PlayerSemaphore.unlockGame();
        }

        public override void enableEventObjects()
        {
            theBoard.btnBankTrade.Enabled = true;
            
            theBoard.btnEndTurn.Enabled = true;
            theBoard.btnBankTrade.Click += bankTrade;
            
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

        public override void disableEventObjects()
        {
            theBoard.btnBankTrade.Enabled = false;

            theBoard.btnEndTurn.Enabled = false;
            theBoard.btnBankTrade.Click -= bankTrade;

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
