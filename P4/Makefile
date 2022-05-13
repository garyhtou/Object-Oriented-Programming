CXX := g++
CXXFLAGS := -g -O0 -std=c++17

SRC	:= gridFlea.cpp inFest.cpp P4.cpp
HDR	:= gridFlea.h inFest.h
OBJ	:= $(patsubst %.cpp, %.o, $(SRC))

all: P4

P4: $(OBJ)
	$(CXX) -o $@ $(OBJ)
%.o:	%.cpp $(HDR)
	$(CXX) $(CXXFLAGS) -c -o $@ $<

clean:
	rm -f P4 *.o
