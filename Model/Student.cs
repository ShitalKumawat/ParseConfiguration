namespace Model
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public Status StudentStatus { get; set; }
        public override string ToString()
        {
            return
                "\n \t\t\t Id :" + this.Id.ToString() +
                "\n \t\t\t Name :" + this.Name.ToString() +
                "\n \t\t\t Department :" + this.Department.ToString() +
                "\n \t\t\t StudentStatus :" + this.StudentStatus.ToString();
        }
    }

    public enum Status
    {
        New,
        Old
    }
}
