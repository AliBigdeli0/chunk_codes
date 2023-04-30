#include "../include/dialog.hpp"

SuccessDialog::SuccessDialog(sf::Font fn, sf::RenderWindow *window)
{
    this->font = fn;
    this->window = window;
}

void SuccessDialog::draw(CircleScores *scores)
{
    std::string playerString = "";
    sf::Color playerColor = sf::Color::Black;

    if (scores->bluePlayerScore > scores->redPlayerScore)
    {
        playerString = "Player 1 Wins!";
        playerColor = PLAYER1_COLOR;
    }
    else if (scores->bluePlayerScore < scores->redPlayerScore)
    {
        playerString = "Player 2 Wins!";
        playerColor = PLAYER2_COLOR;
    }
    else
    {
        playerString = "There Is No Winner!";
    }

    /// dialog
    sf::RectangleShape dialogBox(sf::Vector2f(DIALOG_SUCESS_WIDTH, DIALOG_SUCESS_HEIGHT));
    dialogBox.setFillColor(DRAW_SCORE_BACKGROUND_COLOR);
    dialogBox.setOutlineThickness(2);
    dialogBox.setOutlineColor(playerColor);
    dialogBox.setPosition(50, 50);

    const sf::Vector2f dbPos = dialogBox.getPosition();
    const sf::Vector2f dbSize = dialogBox.getSize();

    /// dialog text
    sf::Text dialogText(playerString.c_str(), this->font, DIALOG_SUCESS_FONT_SIZE);
    dialogText.setFillColor(playerColor);
    sf::FloatRect bounds = dialogText.getLocalBounds();
    dialogText.setPosition(dbPos.x + (dbSize.x - bounds.width) / 2, dbPos.y + DIALOG_SUCESS_TOP_MARGIN);

    /// reset button
    sf::RectangleShape resetButton(DIALOG_SUCESS_RESET_BUTTON_SIZE);
    resetButton.setFillColor(DRAW_SCORE_BACKGROUND_COLOR);
    resetButton.setOutlineThickness(2);
    resetButton.setOutlineColor(sf::Color::White);

    sf::Vector2f rbPos = resetButton.getPosition();
    const sf::Vector2f rbSize = resetButton.getSize();

    resetButton.setPosition(dbPos.x + dbSize.x - rbSize.x - DIALOG_SUCESS_RESET_BUTTON_MARGIN, dbPos.y + dbSize.y - rbSize.y - DIALOG_SUCESS_RESET_BUTTON_MARGIN);

    rbPos = resetButton.getPosition();

    this->resetButtonArea.left = rbPos.x;
    this->resetButtonArea.top = rbPos.y;
    this->resetButtonArea.width = rbSize.x;
    this->resetButtonArea.height = rbSize.y;

    /// reset button text
    sf::Text resetButtonText("RESET", font, 14);
    resetButtonText.setPosition(rbPos.x + 6, rbPos.y + 5);

    window->draw(dialogBox);
    window->draw(dialogText);
    window->draw(resetButton);
    window->draw(resetButtonText);
}

sf::FloatRect SuccessDialog::getResetButtonRect() const
{
    return this->resetButtonArea;
}

bool SuccessDialog::onRestButtonClick() const
{
    sf::Vector2i mousePosition = sf::Mouse::getPosition(*window);
    return this->resetButtonArea.contains(static_cast<sf::Vector2f>(mousePosition));
}