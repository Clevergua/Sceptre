local gameState = {
    frameCount = 0,
}
local mapGenerator = require("MapGenerators.MapGenerator")

function gameState:Start(seed)
    local map = mapGenerator.Generate(seed)
end

function gameState:Update()
    self.frameCount = self.frameCount + 1
end

_G.gameState = gameState

gameState:Start(124214)
