class cCheck {
  public cCheck(int[] arr) {
    if(arr == null) throw new Exception();

    this.arr = new int[arr.Length];
    for(int i = 0; i < arr.Length; i++) {
      if(arr[i] == 0) throw new Exception();
      this.arr[i] = arr[i];
      initArr[i] = arr[i];
    }
  }

  // eh, this is boring

  private int[] arr, initA;



}
