local mathUtils = {}

function mathUtils.CantorPair(a, b)
    return (a + b) * (a + b + 1) // 2 + b
end

function mathUtils.LCGRandom(v)
    return (1140671485 * v + 12820163) % 16777216
end

function mathUtils.Random3(a, b, c)
    local d = mathUtils.CantorPair(a, b)
    return mathUtils.CantorPair(mathUtils.LCGRandom(d), mathUtils.LCGRandom(c))
end

function mathUtils.Random2(a, b)
    local c = mathUtils.CantorPair(a, b)
    return mathUtils.LCGRandom(c)
end

function mathUtils.Dot(x1, y1, x2, y2)
    return x1 * x2 + y1 * y2
end

local xArray = { 1, 2, -1, -2, -1, -2, 1, 2 }
local yArray = { 2, 1, 2, 1, -2, -1, -2, -1 }
local GetGradient2D = function(x, y, seed)
    local hash = mathUtils.Random3(x, y, seed)
    hash = hash > 0 and hash or math.abs(hash)
    local index = hash % 8 + 1
    return xArray[index], yArray[index]
end

local DotGridGradient2D = function(ix, iy, x, y, seed)
    local dx = x - ix
    local dy = y - iy

    local gx, gy = GetGradient2D(ix, iy, seed)
    return dx * gx + dy * gy
end

local SmoothLerp = function(a, b, t)
    t = t * t * t * (6 * t * t - 15 * t + 10)
    return a + (b - a) * t
end

function mathUtils.PerlinNoise2D(seed, x, y)
    -- Determine grid cell coordinates
    local x0 = math.floor(x)
    local x1 = x0 + 1
    local y0 = math.floor(y)
    local y1 = y0 + 1
    -- Determine interpolation weights
    -- Could also use higher order polynomial/s-curve here
    local sx = x - x0
    local sy = y - y0
    -- Interpolate between grid point gradients
    local n0, n1, ix0, ix1, value
    n0 = DotGridGradient2D(x0, y0, x, y, seed)
    n1 = DotGridGradient2D(x1, y0, x, y, seed)
    ix0 = SmoothLerp(n0, n1, sx)
    n0 = DotGridGradient2D(x0, y1, x, y, seed)
    n1 = DotGridGradient2D(x1, y1, x, y, seed)
    ix1 = SmoothLerp(n0, n1, sx)
    value = SmoothLerp(ix0, ix1, sy)
    return value
end

return mathUtils
