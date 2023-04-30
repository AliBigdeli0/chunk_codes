#include "../include/score_board.hpp"
#include "../include/constants.hpp"
#include "../include/log.hpp"

#include <sstream>
#include <string>
#include <iomanip>
#include <iostream>

ScoreBoard::ScoreBoard(sf::RenderWindow *wn)
{
    if (!font.loadFromFile(FONT_FILE_PATH))
    {
        Log::Error("couldn't load font");
    }

    this->window = wn;
}

void ScoreBoard::drawScores(CircleScores scores)
{
    sf::Text player1Text = this->createScoreText(1, BOARD_SQUIR_SIZE, 0, scores.bluePlayerScore);
    sf::RectangleShape player1BackRect = this->createBackgroundRectangle(BOARD_SQUIR_SIZE, 0, this->stringLenght);
    if (scores.playerTurn == PLAYER1_COLOR)
        drawTurnsBorder(&player1BackRect, PLAYER1_COLOR);

    sf::Text player2Text = this->createScoreText(2, WIDTH - BOARD_SQUIR_SIZE, 0, scores.redPlayerScore);
    sf::RectangleShape player2BackRect = this->createBackgroundRectangle(WIDTH - BOARD_SQUIR_SIZE - this->stringLenght, 0, this->stringLenght);
    if (scores.playerTurn == PLAYER2_COLOR)
        drawTurnsBorder(&player2BackRect, PLAYER2_COLOR);

    this->window->draw(player1BackRect);
    this->window->draw(player2BackRect);

    this->window->draw(player1Text);
    this->window->draw(player2Text);
}

sf::Text ScoreBoard::createScoreText(int player, int x, int y, int score)
{
    std::ostringstream playerStream;
    playerStream << "Player " << player << " : " << std::setw(2) << std::setfill('0') << score;

    sf::Text text(playerStream.str().c_str(), this->font, 16);
    this->stringLenght = text.getLocalBounds().width;
    if (player == 1)
    {
        text.setPosition(x, y);
        text.setFillColor(PLAYER1_COLOR);
    }
    else if (player == 2)
    {
        text.setPosition(x - this->stringLenght, y);
        text.setFillColor(PLAYER2_COLOR);
    }
    return text;
}

sf::RectangleShape ScoreBoard::createBackgroundRectangle(int x, int y, int stringLength)
{
    sf::RectangleShape shape;

    shape.setFillColor(DRAW_SCORE_BACKGROUND_COLOR);
    shape.setSize(sf::Vector2f(stringLength + 2 * DRAW_SCORE_BACKGROUND_MARGIN, BOARD_START_SQUIR_Y / 2));
    shape.setPosition(sf::Vector2f(x - 1.5 * DRAW_SCORE_BACKGROUND_MARGIN, y));

    return shape;
}

void ScoreBoard::drawTurnsBorder(sf::RectangleShape *rectangle, sf::Color color)
{
    rectangle->setOutlineThickness(2);
    rectangle->setOutlineColor(color);
}

sf::Font ScoreBoard::getFont()
{
    return font;
}