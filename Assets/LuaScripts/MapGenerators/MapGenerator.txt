local mathUtils = require("MathUtils")
local generator = {}

function generator.Generate(seed)
    local map = {}
    local min, max = 0, 0
    for x = 1, 128 do
        map[x] = {}
        for y = 1, 128 do
            local temperature = mathUtils.PerlinNoise2D(seed + 1, x * 0.007, y * 0.007) * 100
            local humidity = mathUtils.PerlinNoise2D(seed + 2, x * 0.007, y * 0.007) * 100

            if temperature < min then
                min = temperature
            end
            if temperature > max then
                max = temperature
            end

            if temperature < -30 then
                if humidity < -30 then
                    map[x][y] = 1
                elseif humidity < 30 then
                    map[x][y] = 2
                else
                    map[x][y] = 3
                end
            elseif temperature < 30 then
                if humidity < -30 then
                    map[x][y] = 4
                elseif humidity < 30 then
                    map[x][y] = 5
                else
                    map[x][y] = 6
                end
            else
                if humidity < -30 then
                    map[x][y] = 7
                elseif humidity < 30 then
                    map[x][y] = 8
                else
                    map[x][y] = 9
                end
            end
        end
    end
    print("Min temperature:", min)
    print("Max temperature:", max)
    return map
end

return generator
