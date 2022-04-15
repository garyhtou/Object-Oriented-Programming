CXX := g++ 
CXXFLAGS := -g -O0 -std=c++17

SRC	:= gridFlea.cpp inFest.cpp P2.cpp
HDR	:= gridFlea.h inFest.h
OBJ	:= $(patsubst %.cpp, %.o, $(SRC))

all: P2

P2: $(OBJ)
	$(CXX) -o $@ $(OBJ)
%.o:	%.cpp $(HDR)
	$(CXX) $(CXXFLAGS) -c -o $@ $<

clean:
	rm -f P2 *.o
