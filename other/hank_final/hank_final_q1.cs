class simpleD {
  // post-condition: assumes ownership of a array
  public simpleD(int[] a){
    if(a == null || a.Length < 1) throw new Exception();

    this.a = a;
    a = null;
  }

  public virtual int getValue() {
    forward ? index-- : index++;
    int i = abs(index) % a.Length;
    return a[i];
  }

  public void toggle() {
    forward = !forward;
  }

  // post-condition: assumes ownership of z array
  public void reduce(int[] z) {
    if(z == null || z.Length < 1 || z[0] >= a[0]) throw new Exception();

    this.a = z;
    z = null;
  }

  public virtual void scramble(int x) {
    scrambleHelper(x, 1);
  }

  protected int[] a;
  protected bool forward = true;
  protected int index = 0;

  protected void scrambleHelper(int x, int step) {
    for(int i = 0; i < a.Length; i += step) {
      a[i] += (forward ? 1 : -1) * x;
    }
  }
}


class alterD : simpleD {
  public alterD(int[] a) : base(a) {}

  public override void scramble(x) {
    scrambleHelper(x, 2);
  }
}


class compoundD : simpleD {
  public compoundD(int[] a) : base(a) {}

  public override void getValue() {
    int i1 = abs(index) % a.Length;
    int i2 = abs(index+1) % a.Length;
    return a[i1] + a[i2];
  }
}
