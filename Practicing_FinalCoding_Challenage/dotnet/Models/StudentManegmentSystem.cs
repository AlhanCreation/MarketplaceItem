namespace dotnet.Models{
	public class StudentManegmnetSystem{
		public int StudentID {get;set;}
		public string? StudentName {get;set;}

		public string? Subject {get; set;}

		public int MarkObtain {get;set;}

		public int TotalMark{get;set;}

		public int Percentage{get;set;}

		public  StudentManegmnetSystem(string? StudentName,string? Subject,int MarkObtain,int TotalMark){
			this.StudentName = StudentName;
			this.Subject = Subject;
			this.MarkObtain = MarkObtain;
			this.TotalMark = TotalMark;
		}
	}
}