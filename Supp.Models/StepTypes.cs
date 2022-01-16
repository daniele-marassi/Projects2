using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public enum StepTypes
    {
        Default = 0,
        ApplyNow = 2,
        ApplyManually = 3,
        Choice = 4,
        GetElementValue = 5,
        Execute = 6,
        Ask = 7
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