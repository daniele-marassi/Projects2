using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public enum StepTypes
    {
        Default = 0,
        GetAnswer = 1,
        AddNow = 2,
        AddManually = 3,
        Choice = 4,
        GetElementName = 5,
        GetElementValue = 6
    }

    public class StepTypesUtility
    {
        /// <summary>
        /// Get StepTypes
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<StepType> Get()
        {
            var stepTypes = new List<StepType>();
            var values = Enum.GetValues(typeof(StepTypes));
            foreach (var item in values)
            {
                stepTypes.Add(new StepType() { Id = item.ToString(), Name = item.ToString() });
            }

            stepTypes = stepTypes.OrderBy(_ => _.Name).ToList();

            return stepTypes;
        }
    }

    public class StepType
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}