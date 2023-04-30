#pragma once

#include <iostream>
class Log
{
public:
    static void Info(const char *message);
    static void Error(const char *message);

private:
    static char *getTime();
};