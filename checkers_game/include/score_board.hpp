#pragma once

#include <SFML/Graphics.hpp>

#include "../include/circles.hpp"

class ScoreBoard
{
public:
    ScoreBoard(sf::RenderWindow *);
    void drawScores(CircleScores);
    sf::Font getFont();

private:
    sf::Text createScoreText(int, int, int, int);
    sf::RectangleShape createBackgroundRectangle(int, int, int);
    void drawTurnsBorder(sf::RectangleShape *, sf::Color);
    sf::Font font;
    sf::RenderWindow *window;
    int stringLenght;
};