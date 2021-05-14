using System.Collections.Generic;

namespace Mair.DS.Engines.Core.EventManager
{
    public interface IEventManager
    {
        string Name { get; set; }

        void Init();
        void CheckConditions(object sender, object e);
        List<string> GetConfiguration();
    }
}
