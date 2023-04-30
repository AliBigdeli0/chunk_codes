#pragma once
#include <vector>
#include <SFML/Graphics.hpp>

#include "constants.hpp"
////////////////////////////
class Cell
{
public:
    Cell()
    {
        reset();
    };

    void setVisited(bool vs) { this->visited = vs; }
    bool getVisited() { return visited; }
    bool isCellFill() { return this->visited == true; }
    void setColor(sf::Color color) { this->color = color; }
    sf::Color getColor() { return this->color; }
    bool isColorDifferentFromPlayer(sf::Color playerColor) { return this->visited && this->color == playerColor; }
    void reset()
    {
        this->visited = false;
        this->color = sf::Color::Transparent;
    }

private:
    bool visited;
    sf::Color color;
};

//////////////////////////////
class CircleScores
{
public:
    int redPlayerScore = 0;
    int bluePlayerScore = 0;
    sf::Color playerTurn = sf::Color::Transparent;

    CircleScores(){};
};

//////////////////////////////
class Circles
{
public:
    Circles(sf::RenderWindow *);
    void setCircle(int x, int y);
    CircleScores drawCircle();
    Cell *cells[BOARD_ROWS + 1][BOARD_COLS + 1];
    bool isAllCellsVisited();
    void reset();

private:
    void checkCells(sf::Color, int, int);
    void checkCellsVerticalForwrad(sf::Color, int, int);
    void checkCellsVertialBackward(sf::Color, int, int);
    void checkCellsHorizontalForwrad(sf::Color, int, int);
    void checkCellsHorizontalBackward(sf::Color, int, int);
    sf::Color getCurrentPlayerColor();
    bool isPlayer1Turn();
    bool playerSwitch();

    void scoreCounter(CircleScores *, sf::Color);
    sf::RenderWindow *window;
    bool playerTurn = false;
    bool isallVisited = false;
};