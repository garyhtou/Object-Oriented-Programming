CXX := g++ 
CXXFLAGS := -g -O0 -std=c++17

SRC	:= GridFlea.cpp
HDR	:= GridFlea.h
# SRC	:= GridFlea.cpp Infest.cpp P1.cpp
# HDR	:= GridFlea.h Infest.h
OBJ	:= $(patsubst %.cpp, %.o, $(SRC))

all: p1

p1: $(OBJ)
	$(CXX) -o $@ $(OBJ)
%.o:	%.cpp $(HDR)
	$(CXX) $(CXXFLAGS) -c -o $@ $<

clean:
	rm -f P1 *.o
