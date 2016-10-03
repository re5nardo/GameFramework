
namespace Navigation
{
	public class Defines
	{
		public const int 			INVALID_NODE_INDEX = -1;
		public const int 			NO_CLOSEST_NODE_FOUND = -1;
		public const double 		SMAllEST_DELAY = 0.25;
	}

	public enum SearchResult
	{
		target_found,
		target_not_found,
		search_incomplete,
	}
}