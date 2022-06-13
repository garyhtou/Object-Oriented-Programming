class abstract cd {
  protected checkSimple(cCheck c, simpleD d) {
    this.c = c;
    this.d = d;
  }

  public int getValue() {
    return d.getValue();
  }
  public void toggle() {
    d.toggle();
  }
  public void reduce(int[] z) {
    d.reduce(z);
  }
  public void scramble(int x) {
    d.scramble(x);
  }
  public int[] half() {
    return c.half();
  }
  public void rest() {
    return c.reset();
  }
  public void perturb() {
    return c.perturb();
  }

  private readonly cCheck c;
  private readonly simpleD d;
}

class cCheckSimpleD : cd {
  public cCheckSimpleD(int[] x) : base(new cCheck(x), new simpleD(x))
}

class cCheckAlterD : cd {
  public cCheckSimpleD(int[] x) : base(new cCheck(x), new alterD(x))
}

class cCheckCompoundD : cd {
  public cCheckSimpleD(int[] x) : base(new cCheck(x), new compoundD(x))
}

// Client Code

public static void main() {
  cd[] cds = getCds();

  foreach(cd c in cds) {
    c.getValue();
    c.toggle();
    c.reduce(new int[] {1, 5, 6});
    c.scramble(10);
    c.half();
    c.rest();
    c.preturb();
  }
}

private static cd[] getCds() {
  int len = 10;
  cd[] arr = new cd[len];
  for(int i = 0; i < len; i++) {
    arr[i] = getCd();
  }
  return arr;
}

private static cd[] getInput() {
  int len = r.Next(5, 10);
  int[] arr = new int[len];
  for(int i = 0; i < len; i++) {
    arr[i] = r.Next(-5, 5);
  }
  return arr;
}

const numClasses = 3;
private static cd getCd() {
  int index = r.Next(0, 3);
  switch(index) {
    case 0:
      return new cCheckSimpleD(getInput());
      break;
    case 1:
      return new cCheckAlterD(getInput());
      break;
    case 2:
      return new cCheckCompoundD(getInput());
      break;
  };
}
private Random r = new Random();
