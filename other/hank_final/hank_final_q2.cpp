public class cCheck {
public:
  cCheck(int* arr, int length) {
    if(arr == nullptr) throw new Exception();

    this.arr = new int[length];
    initArr = new int[length];
    for(int i = 0; i < length; i++) {
      if(arr[i] == 0) throw new Exception();
      this.arr[i] = arr[i];
      initArr[i] = arr[i];
    }
  }

  int* half() {
    int start = firstHalf ? 0 : (length+1) / 2;
    int end = firstHalf ? (length+1) / 2 : length;

    int* ret = new int[length / 2];
    for(int i = start; i < end; i++) {
      ret[i] = arr[i];
    }
    firstHalf = !firstHalf;
    return ret;
  }

  void reset() {
    firstHalf = true;
    for(int i = 0; i < length; i++) {
      arr[i] = initArr[i];
    }
  }

  void perturb() {
    for(int i = 0; i < length; i++) {
      if(arr[i] % 2 == 0) {
        arr[i]++;
      } else {
        arr[i] *= 2;
      }
    }
  }

  cCheck operator+=(int x) {
    for(int i = 0; i < length; i++) {
      arr[i] += x;
    }
    return *this;
  }

  cCheck operator-=(int x) {
    +=(-x);
    return *this;
  }

  cCheck operator+(int x) const {
    cCheck temp = cCheck(*this);
    temp += x;
    return temp;
  }

  cCheck operator-(int x) const {
    cCheck temp = cCheck(*this);
    temp -= x;
    return temp;
  }

  cCheck operator++ {
    +=(1);
    return *this;
  }

  cCheck operator++(int x) {
    cCheck temp = cCheck(*this);
    +=(1);
    return temp;
  }

  cCheck operator-- {
    -=(1);
    return *this;
  }

  cCheck operator--(int x) {
    cCheck temp = cCheck(*this);
    -=(1);
    return temp;
  }

  // destructor, copy, and move semantics

private:
  int* arr, initArr;
  int length;
  bool firstHalf;
}

