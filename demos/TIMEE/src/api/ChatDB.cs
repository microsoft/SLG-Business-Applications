using System;
using System.Collections.Generic;
using TimHanewich.AgentFramework;

namespace TIMEEAPI
{
    public class ChatHistory
    {
        public string Key { get; set; }
        public List<Message> History { get; set; }

        public ChatHistory()
        {
            Key = "";
            History = new List<Message>();
        }
    }

    public class ChatDB
    {
        private List<ChatHistory> _ChatHistory;

        public ChatDB()
        {
            _ChatHistory = new List<ChatHistory>();
        }

        public List<Message> Retrieve(string key)
        {
            foreach (ChatHistory ch in _ChatHistory)
            {
                if (ch.Key == key)
                {
                    return ch.History;
                }
            }
            return new List<Message>();
        }

        public void Save(string key, Message[] messages)
        {
            foreach (ChatHistory ch in _ChatHistory)
            {
                if (ch.Key == key)
                {
                    ch.History = messages.ToList();
                    return;
                }
            }
            _ChatHistory.Add(new ChatHistory() { Key = key, History = messages.ToList() });
        }

    }
}