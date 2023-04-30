#include "../include/sound.hpp"
#include "../include/log.hpp"
#include "../include/constants.hpp"

#include <iostream>

Sound::Sound()
{
    if (!this->itemPutSound.openFromFile(SOUND_PUT_ITEM))
    {
        Log::Error("sound not loaded");
    }
    this->itemPutSound.setVolume(25);
    this->itemPutSound.stop();
}

void Sound::delayMs(int ms)
{
    sf::Clock Timer;
    while (Timer.getElapsedTime().asMilliseconds() < ms)
        ;
}

void Sound::playItemPut()
{
    itemPutSound.stop();
    itemPutSound.play();
    delayMs(300);
}
