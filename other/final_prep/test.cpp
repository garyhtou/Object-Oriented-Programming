class A {
  public A(A other){
    // ...
  };

  public virtual void ~A() {};

  public operator+(int b) const {
  };

  public B operator+(const B b) const {
    return B;
  };

  public A operator++(int x) { // post
    A temp = A(*this);
    this.some_var++; // do something
    return temp;
  };
}

// Global
A operator+(int b, const A a) const {
  return a + b;
}



