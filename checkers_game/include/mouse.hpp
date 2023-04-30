#pragma once

#include <SFML/Graphics.hpp>

class Mouse
{
public:
    Mouse(sf::RenderWindow *);
    sf::IntRect getMatrixPosition();

private:
    bool isInBoard(sf::Vector2i);
    sf::RenderWindow *window;
};