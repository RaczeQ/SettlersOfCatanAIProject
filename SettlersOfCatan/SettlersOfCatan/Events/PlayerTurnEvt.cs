﻿using System;
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
                    }
                    break;
                case State.Trade:
                    //This should be unreachable.
                    break;

            }
        }

        public void bankTrade(Object sender, EventArgs e)
        {
            disableEventObjects();
            tradeWindow = new TradeWindow();
            tradeWindow.loadBankTrade();
            currentState = State.Trade;
            tradeWindow.Show();
        }

        public void harborTrade(Object sender, EventArgs e)
        {
            disableEventObjects();
            tradeWindow = new TradeWindow();
            tradeWindow.loadHarborTrade();
            currentState = State.Trade;
            tradeWindow.Show();
        }
        
        public void playerTrade(Object sender, EventArgs e)
        {
            disableEventObjects();
            tradeWindow = new TradeWindow();
            tradeWindow.loadPlayerTrade();
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
        }

        public void disableEventObjects()
        {
            theBoard.btnBankTrade.Enabled = false;
            theBoard.btnPlayerTrade.Enabled = false;
            theBoard.btnEndTurn.Enabled = false;
            theBoard.btnBankTrade.Click -= bankTrade;
            theBoard.btnPlayerTrade.Click -= playerTrade;
        }
    }
}
