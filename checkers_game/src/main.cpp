#include <SFML/Graphics.hpp>
#include <SFML/System.hpp>
#include <iostream>
#include <string>

#include "../include/constants.hpp"
#include "../include/assets.hpp"
#include "../include/log.hpp"
#include "../include/mouse.hpp"
#include "../include/circles.hpp"
#include "../include/score_board.hpp"
#include "../include/sound.hpp"
#include "../include/dialog.hpp"

void setWindowAtCenter(sf::RenderWindow *window)
{
    // set screen center
    sf::VideoMode desktopMode = sf::VideoMode::getDesktopMode();
    int winPosX = (desktopMode.width - WIDTH) / 2;
    int winPosY = (desktopMode.height - HEIGHT) / 2;

    window->setPosition(sf::Vector2i(winPosX, winPosY));
}

void printInfo()
{
    char buffer[1024];
    std::sprintf(buffer, "SFML version is: %d.%d.%d", SFML_VERSION_MAJOR, SFML_VERSION_MINOR, SFML_VERSION_PATCH);
    Log::Info(buffer);
}

int main(int argc, char const *argv[])
{
    printInfo();

    sf::ContextSettings settings;
    settings.antialiasingLevel = 8;

    sf::RenderWindow window(sf::VideoMode(WIDTH, HEIGHT), "Game", sf::Style::Titlebar | sf::Style::Close, settings);
    setWindowAtCenter(&window);
    window.setFramerateLimit(60);

    sf::Texture *backgroundTexture = new sf::Texture();
    sf::Sprite *backgroundSprite = new sf::Sprite();
    Assets::LoadBackground(backgroundTexture, backgroundSprite);

    Mouse mouse(&window);
    Circles circles(&window);
    ScoreBoard scoreBoard(&window);
    Sound sound;
    SuccessDialog successDialog(scoreBoard.getFont(), &window);

    while (window.isOpen())
    {
        sf::Event event;

        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
            {
                Log::Info("going to close window");
                window.close();
            }

            if (event.type == sf::Event::MouseButtonPressed)
            {
                if (!circles.isAllCellsVisited())
                {
                    sf::IntRect pos = mouse.getMatrixPosition();
                    if (pos.left != BOARD_INVALID_POSITION)
                    {
                        if (!circles.cells[pos.left][pos.top]->getVisited())
                            sound.playItemPut();
                        circles.setCircle(pos.left, pos.top);
                    }
                }
                else
                {
                    if (successDialog.onRestButtonClick())
                    {
                        circles.reset();
                    }
                }
            }
        }

        window.clear(sf::Color::Black);
        window.draw(*backgroundSprite);

        // draw objects
        CircleScores scores = circles.drawCircle();
        scoreBoard.drawScores(scores);
        if (circles.isAllCellsVisited())
        {
            successDialog.draw(&scores);
        }

        window.display();
    }

    return 0;
}
