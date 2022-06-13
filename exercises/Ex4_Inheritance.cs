public class Slider {
	public Slider(x, y){}

	protected int sizeX;
	protected int sizeY;
	protected bool state = true;

	public virtual move(){}

	public virtual getInterval(){}

	public virtual expand(){}
	public virtual shrink(){}

	public disintegrate(){}
	public newInterval(){}
}


public class Expander : Slider {
	int expandLimit;

	public override expand(){}
	public override shrink(){}
}


public class Inverter : Slider {
	public override move(){}
	public override getInterval(){}
}
