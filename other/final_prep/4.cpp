// Question a
class Polygon {
  public:
    virtual double area() = 0;
    virtual void int() = 0;
}

// Question b
class Square : public Polygon {
  double width;
public:
  Square(double width) {
    this.width = width;
  };
  override double area () {
    return width * width;
  };
  override void print () {
    printf("Square(%f)\n", width)
  };
}

