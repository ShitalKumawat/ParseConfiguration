using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Batch
    {
        public int Id { get; set; }
        public int StudentCount { get; set; }
        public List<Student> Students { get; set; }

        public override string ToString()
        {
            var result =
                 "\n \t\t Id :" + this.Id.ToString() +
                "\n \t\t StudentCount :" + this.StudentCount.ToString() +
                "\n \t\t Students :" ;
            foreach (var student in Students)
            {
                result += student.ToString();
            }
            return result;
        }
    }
}
