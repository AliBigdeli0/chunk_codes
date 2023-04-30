#include "../include/assets.hpp"
#include "../include/constants.hpp"
#include "../include/log.hpp"

void Assets::LoadBackground(sf::Texture *backgroundTexture, sf::Sprite *backgroundSprite)
{
    if (!backgroundTexture->loadFromFile("assets/background.jpg"))
    {
        Log::Error("error in loading background texture");
        return;
    }

    if (backgroundTexture->getSize() == sf::Vector2u(0, 0))
    {
        Log::Error("size of texxture is zero");
        return;
    }

    Log::Info("background texture loaded");

    // create background sprite
    backgroundSprite->setTexture(*backgroundTexture);

    sf::IntRect rect(BOARD_LOCATION_X, BOARD_LOCATION_Y, BOARD_SIZE, BOARD_SIZE);
    backgroundSprite->setTextureRect(rect);

    // set scale
    sf::FloatRect bounds = backgroundSprite->getLocalBounds();
    backgroundSprite->setScale(WIDTH / bounds.width, HEIGHT / bounds.height);
}