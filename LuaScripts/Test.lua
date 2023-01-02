local mathUtils = require("MathUtils")
local min = 0
local max = 0
for seed = 1, 1 do
    for x = 1, 200 do
        for y = 1, 200 do
            local value = mathUtils.PerlinNoise2D(seed, x * 0.017, y * 0.017)
            print(value)
            if value < min then
                min = value
            end
            if value > max then
                max = value
            end
        end
    end
end
print(66666)
print(111111)
print(2222222)print(2222222)print(2222222)