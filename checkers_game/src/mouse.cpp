#include "../include/mouse.hpp"
#include "../include/constants.hpp"
#include "../include/log.hpp"

Mouse::Mouse(sf::RenderWindow *wd)
{
    this->window = wd;
}

sf::IntRect Mouse::getMatrixPosition()
{
    sf::Vector2i mp = sf::Mouse::getPosition(*this->window);
    sf::IntRect invalidPosition(BOARD_INVALID_POSITION, BOARD_INVALID_POSITION, BOARD_INVALID_POSITION, BOARD_INVALID_POSITION);
    if (!this->isInBoard(mp))
    {
        return invalidPosition;
    }

    mp.x = mp.x - BOARD_START_SQUIR_X;
    mp.y = mp.y - BOARD_START_SQUIR_Y;

    sf::IntRect res = sf::IntRect(0, 0, mp.x, mp.y);
    res.left = (mp.x - (mp.x % BOARD_START_SQUIR_X)) / BOARD_START_SQUIR_X;
    res.top = (mp.y - (mp.y % BOARD_START_SQUIR_Y)) / BOARD_START_SQUIR_Y;

    return res;
}

bool Mouse::isInBoard(sf::Vector2i position)
{
    int x = position.x;
    int y = position.y;
    sf::Vector2u ws = this->window->getSize();

    if (x < BOARD_START_SQUIR_X)
        return false;
    if (y < BOARD_START_SQUIR_Y)
        return false;
    if (x > (ws.x - BOARD_START_SQUIR_X))
        return false;
    if (y > (ws.y - BOARD_START_SQUIR_Y))
        return false;
    return true;
}