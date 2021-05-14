using Mair.DS.Engines.Core.EventManager;
using Mair.DS.Models.Entities.Automation;
using System.Collections.Generic;

namespace Mair.DS.Engines.TagDispatcher
{
    public interface ITagDispatcher
    {
        string Name { get; set; }
        EventManagerEventHandler Notifier { get; set; }
        void Init();
        TagValue ReadTagValue(Tag tag);
        List<TagValue> ReadAllTagsValue();
        string WriteTagValue(Tag tag, string value);
        bool SubscribeDataChange(Tag tag);
        bool UnsubscribeDataChange(Tag tag);
    }
}