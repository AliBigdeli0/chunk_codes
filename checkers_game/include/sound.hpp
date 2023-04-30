#pragma once

#include <SFML/Audio.hpp>

class Sound
{
public:
    Sound();
    void playItemPut();

private:
    sf::Music itemPutSound;
    void delayMs(int);
};