using System;
namespace RailRoadCounter
{
	public static class RequestData
	{
		public static Station DepartureStation { get; set; }
		public static Station ArrivalStation { get; set; }

		public static Cargo Cargo { get; set; }

		public static double DepartureWeight { get; set; }
		public static int NumOfWagons { get; set; }
		public static int NumOfAxis { get; set; }
		public static int NumOfGuardedWagons { get; set; } 
		public static int NumOfConductors { get; set; }

	}
}
