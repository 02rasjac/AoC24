#define IS_SAMPLE

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] data = File.ReadAllLines(path);

// Use A* algorithm (https://www.geeksforgeeks.org/a-search-algorithm/)
// In the double for-loop, we can only move in 4 directions, so `continue` when `!(i == 0 || j == 0) || (i == 0 && j == 0)`.
//
// When the example calculates `gNew`, it adds `1.0` to the current cells cost from start. We want to add
//* 1 if the new cell is in the direction of travel
//* 1001 if the new cell requires turning.
// Example of cost (g):
// ##        #      ############
// #1002    2003    ...#...#.#.#
// #1001    #       ##.#.#.#.#.#
// #S       1       2#.....#...#
// ##       #       ############
//
// We can get the direction of travel from the parent-nodes coordinate