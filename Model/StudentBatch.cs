using System.Collections.Generic;

namespace Model
{
    public class StudentBatch
    {
        public int BatchCount { get; set; }
        public List<Batch> Batches { get; set; }

        public override string ToString()
        {
            var result = 
                "\n BatchCount :" + this.BatchCount.ToString()+
                "\n \t Batches :";
            foreach (var batch in Batches)
            {
                result += batch.ToString();
            }
            return result;
        }
    }
}