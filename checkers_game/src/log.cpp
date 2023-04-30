#include "../include/log.hpp"

#include <ctime>

char buffer[80];

char *Log::getTime()
{
    std::time_t currentTime = std::time(nullptr);

    // convert the current time to local time
    std::tm *localTime = std::localtime(&currentTime);

    // format the current date and time as a string

    std::strftime(buffer, 80, "%Y-%m-%d %H:%M:%S", localTime);
    return buffer;
}

void Log::Info(const char *message)
{
    std::cout << "[" << getTime() << "][INFO] " << message << std::endl;
}

void Log::Error(const char *message)
{
    std::cerr << "[" << getTime() << "][ERROR] " << message << std::endl;
}