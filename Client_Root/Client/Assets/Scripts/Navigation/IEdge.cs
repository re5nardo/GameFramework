
public interface IEdge
{
	int   	From ();
	void  	SetFrom (int NewIndex);

	int   	To();
	void  	SetTo (int NewIndex);

	double 	Cost ();
	void   	SetCost (double NewCost);
}
