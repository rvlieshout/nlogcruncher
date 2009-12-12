#region Copyright

// The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in compliance
//  with the License. You may obtain a copy of the License at
//  
//  http://www.mozilla.org/MPL/
//  
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations under 
//  the License.
//  
//  The Initial Developer of the Original Code is Robert Smyth.
//  Portions created by Robert Smyth are Copyright (C) 2008.
//  
//  All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI
{
    public class EventsFormatterData : IEventsFormatterData
    {
        private readonly IEventListener<FormatChanged> formatChangedListener;
        private TimeStampFormat timeFormat;
        private bool showContextDepth;

        public EventsFormatterData(IEventListener<FormatChanged> formatChangedListener)
        {
            this.formatChangedListener = formatChangedListener;
            timeFormat = TimeStampFormat.Absolute;
            HiddenMessages = new List<string>();
            HiddenMessageContexts = new Dictionary<IEventContext, bool>();
        }

        public LogEvent ReferenceLogEvent { get; set; }

        public TimeStampFormat TimeFormat
        {
            get { return timeFormat; }
            set
            {
                timeFormat = value;
                formatChangedListener.OnChange();
            }
        }

        public void HideMessages(string message)
        {
            HiddenMessages.Add(message);
            formatChangedListener.OnChange();
        }

        public void HideMessagesInContext(IEventContext context)
        {
            HiddenMessageContexts.Add(context, true);
            formatChangedListener.OnChange();
        }

        public void ShowAllEvents()
        {
            HiddenMessages.Clear();
            HiddenMessageContexts.Clear();
        }

        public bool ShowContextDepth
        {
            set
            {
                showContextDepth = true;
                formatChangedListener.OnChange();
            }
            get { return showContextDepth; }
        }

        public bool EventIsHidden(ILogEvent logEvent)
        {
            return HiddenMessages.Contains(logEvent.Message) || HiddenMessageContexts.ContainsKey(logEvent.Context);
        }

        private List<string> HiddenMessages { get; set; }
        private Dictionary<IEventContext, bool> HiddenMessageContexts { get; set; }
    }
}