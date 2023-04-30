#include "../include/circles.hpp"
#include "../include/log.hpp"

#include <iostream>
Circles::Circles(sf::RenderWindow *wd)
{
    for (int coli = 0; coli < BOARD_COLS; coli++)
    {
        for (int rowi = 0; rowi < BOARD_ROWS; rowi++)
        {
            this->cells[rowi][coli] = new Cell();
            this->cells[rowi][coli]->setVisited(false);
        }
    }
    this->window = wd;
}

void Circles::setCircle(int x, int y)
{
    Cell *cell = this->cells[x][y];

    // cell is fill
    if (cell->isCellFill())
        return;

    cell->setVisited(true);

    sf::Color color = this->getCurrentPlayerColor();
    cell->setColor(color);
    this->checkCells(color, x, y);
    this->playerSwitch();
}

CircleScores Circles::drawCircle()
{
    CircleScores score;
    this->isallVisited = true;

    for (int coli = 0; coli < BOARD_COLS; coli++)
    {
        for (int rowi = 0; rowi < BOARD_ROWS; rowi++)
        {
            Cell *cell = this->cells[rowi][coli];
            if (cell->getVisited())
            {
                sf::CircleShape circle((BOARD_SQUIR_SIZE / 2) - BOARD_CIRCLE_MARGIN);
                sf::Color color = cell->getColor();

                circle.setFillColor(color);
                this->scoreCounter(&score, color);

                circle.setPosition(
                    (rowi + 1) * BOARD_SQUIR_SIZE + BOARD_CIRCLE_MARGIN,
                    (coli + 1) * BOARD_SQUIR_SIZE + BOARD_CIRCLE_MARGIN);

                window->draw(circle);
            }
            else
            {
                this->isallVisited = false;
            }
        }
    }

    score.playerTurn = this->getCurrentPlayerColor();
    return score;
}

void Circles::checkCells(sf::Color playerColor, int row, int col)
{
    checkCellsVerticalForwrad(playerColor, row, col);
    checkCellsVertialBackward(playerColor, row, col);
    checkCellsHorizontalForwrad(playerColor, row, col);
    checkCellsHorizontalBackward(playerColor, row, col);
}

void Circles::checkCellsVerticalForwrad(sf::Color playerColor, int row, int col)
{
    if (row + 2 > BOARD_ROWS)
        return;

    int foundPos = BOARD_INVALID_POSITION;

    for (int i = row + 2; i < BOARD_ROWS; i++)
    {
        Cell *cell = this->cells[i][col];

        if (cell->getColor() == playerColor)
        {
            foundPos = i;
        }

        if (cell->isColorDifferentFromPlayer(playerColor))
        {
            break;
        }
    }

    if (foundPos == BOARD_INVALID_POSITION)
        return;

    for (int i = row + 1; i < foundPos; i++)
    {
        this->cells[i][col]->setColor(playerColor);
        this->cells[i][col]->setVisited(true);
    }
}

void Circles::checkCellsVertialBackward(sf::Color playerColor, int row, int col)
{
    if (row - 2 < 0)
        return;

    int foundPos = BOARD_INVALID_POSITION;

    for (int i = row - 2; i > -1; i--)
    {
        Cell *cell = this->cells[i][col];

        if (cell->getColor() == playerColor)
        {
            foundPos = i;
        }

        if (cell->isColorDifferentFromPlayer(playerColor))
        {
            break;
        }
    }

    if (foundPos == BOARD_INVALID_POSITION)
        return;

    for (int i = row - 1; i > foundPos - 1; i--)
    {
        this->cells[i][col]->setColor(playerColor);
        this->cells[i][col]->setVisited(true);
    }
}

void Circles::checkCellsHorizontalForwrad(sf::Color playerColor, int row, int col)
{
    if (col + 2 > BOARD_COLS)
        return;

    int foundPos = BOARD_INVALID_POSITION;

    for (int i = col + 2; i < BOARD_COLS; i++)
    {
        Cell *cell = this->cells[row][i];

        if (cell->getColor() == playerColor)
        {
            foundPos = i;
        }

        if (cell->isColorDifferentFromPlayer(playerColor))
        {
            break;
        }
    }

    if (foundPos == BOARD_INVALID_POSITION)
        return;

    for (int i = col + 1; i < foundPos; i++)
    {
        this->cells[row][i]->setColor(playerColor);
        this->cells[row][i]->setVisited(true);
    }
}

void Circles::checkCellsHorizontalBackward(sf::Color playerColor, int row, int col)
{
    if (col - 2 < 0)
        return;

    int foundPos = BOARD_INVALID_POSITION;

    for (int i = col - 2; i > -1; i--)
    {
        Cell *cell = this->cells[row][i];

        if (this->cells[row][i]->getColor() == playerColor)
        {
            foundPos = i;
        }

        if (cell->isColorDifferentFromPlayer(playerColor))
        {
            break;
        }
    }

    if (foundPos == BOARD_INVALID_POSITION)
        return;

    for (int i = col - 1; i > foundPos - 1; i--)
    {
        this->cells[row][i]->setColor(playerColor);
        this->cells[row][i]->setVisited(true);
    }
}

void Circles::scoreCounter(CircleScores *score, sf::Color color)
{
    if (color == PLAYER1_COLOR)
    {
        score->bluePlayerScore++;
    }
    else if (color == PLAYER2_COLOR)
    {
        score->redPlayerScore++;
    }
}

sf::Color Circles::getCurrentPlayerColor()
{
    if (this->isPlayer1Turn())
    {
        return PLAYER1_COLOR;
    }
    else
    {
        return PLAYER2_COLOR;
    }
}

bool Circles::isPlayer1Turn()
{
    return playerTurn == false;
}

bool Circles::playerSwitch()
{
    playerTurn = !playerTurn;
    return playerTurn;
}

bool Circles::isAllCellsVisited()
{
    return this->isallVisited;
}

void Circles::reset()
{
    for (int coli = 0; coli < BOARD_COLS; coli++)
    {
        for (int rowi = 0; rowi < BOARD_ROWS; rowi++)
        {
            Cell *cell = this->cells[rowi][coli];
            cell->reset();
        }
    }
}