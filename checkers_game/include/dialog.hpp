#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>

#include "../include/circles.hpp"

class SuccessDialog
{
public:
    SuccessDialog(sf::Font, sf::RenderWindow *);
    void draw(CircleScores *);
    sf::FloatRect getResetButtonRect() const;
    bool onRestButtonClick() const;

private:
    sf::Font font;
    sf::RenderWindow *window;
    sf::FloatRect resetButtonArea;
};