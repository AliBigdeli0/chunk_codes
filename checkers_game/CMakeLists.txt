cmake_minimum_required(VERSION 3.12)

project(SimpleGame)

set(CMAKE_CXX_STANDARD 11)

find_package(SFML 2.5.1 COMPONENTS graphics audio REQUIRED)

file(GLOB SOURCES "src/*.cpp")

include_directories("./include")

add_executable(SimpleGame ${SOURCES})

add_custom_command(TARGET SimpleGame PRE_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy ${CMAKE_SOURCE_DIR}/assets/background.jpg ${CMAKE_BINARY_DIR}/assets/background.jpg)

add_custom_command(TARGET SimpleGame PRE_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy ${CMAKE_SOURCE_DIR}/assets/fonts/TechnoRaceItalic-eZRWe.otf ${CMAKE_BINARY_DIR}/assets/fonts/TechnoRaceItalic-eZRWe.otf)

add_custom_command(TARGET SimpleGame PRE_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy ${CMAKE_SOURCE_DIR}/assets/sound/itemSound.wav ${CMAKE_BINARY_DIR}/assets/sound/itemSound.wav)

target_link_libraries(SimpleGame sfml-graphics sfml-window sfml-audio sfml-system)

add_custom_command(TARGET SimpleGame POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E echo "going to run the program..."
    COMMAND ${CMAKE_BINARY_DIR}/SimpleGame
)