# Mint
Mooshua's Entity Compiler: A source engine entity postcompiler.

## Features
- **Incremental Compilation:** the old model and entity lumps are stored in a map's pakfile, allowing all compilations to be incremental, reversible, and well-documented.
- **Lua Programming:** Use Lua to define entities and create complex relationships between hundreds of entities without a sweat. Use Lua modules to share entities and generator functions. Make the edict cry.
- **Fast:** Compiled and loading into the game in under a second.

## Example

A lua script creating a "simon says" game defined by several blocks for standing on and a display which tells players what color to stand on.

```lua
return function()

    local Case = Prefix:Unique();
    local Timer = Prefix:Unique();

    local Pad = Prefix:Unique()
    local Answer = Prefix:Unique()

    local CaseInstance = Entities:New("logic_case")
        :SetTarget(Case)

    return {
        Case = Case;
        Timer = Timer;
        Pad = Pad;
        Answer = Answer;

        ---
        --- Sets the current "answer" pads to the list
        --- List is in format ["pad_name"] = answer_block
        --- Answer block gets shown first.
        ---
        SetPads = function(self, list, tplocation)

            --  Sync pads and answers to pad/answer prefixes
            for pad, ans in pairs(list) do
                print(pad,ans)
                pad:SetTarget(Pad:Extend(Prefix:Unique()))
                ans:SetTarget(Answer:Extend(Prefix:Unique()))

                if tplocation then
                    ans.Values["origin"] = tplocation.Values["origin"]
                end
            end 

            --  Fun time
            --  We go over each pad/ans pair
            --  We set our own answer to be active,
            --  Then disable all other pads after a short delay
            local CaseIdx = 0
            for this_stays, this_answer in pairs(list) do
                CaseIdx = CaseIdx + 1

                local Output = "OnCase".. (( CaseIdx < 9 and "0" ) or "") .. tostring(CaseIdx)

                --  Enable this answer
                CaseInstance:AddEvent {
                    Output = Output,
                    Entity = this_answer.Target,
                    Input = "Enable",
                }

                for this_gets_removed, _ in pairs(list) do

                    if (this_gets_removed == this_stays) then
                        --  Skip
                    else
                        CaseInstance:AddEvent {
                            Output = Output,
                            Entity = this_gets_removed.Target,
                            Input = "Disable",
                            Delay = 2,
                        }
                    end

                end

            end

        end;

        ---
        --- Creates a new timer and returns the entity associated with it.
        --- This allows the colors script to optimally configure it
        CreateTimer = function(self)
            local timer = Entities:New("logic_timer")
            :SetTarget(Timer)
            --:SetKey("spawnflags",1) --  Set to oscillate
            :SetKey("RefireTime", 5)
            :AddEvent {
                Output = "OnTimer",
                Entity = Case,
                Input = "PickRandom",
                Delay = 0.1
            }
            :AddEvent {
                Output = "OnTimer",
                Entity = Answer:Wildcard(),
                Input = "Disable"
            }
            :AddEvent {
                Output = "OnTimer",
                Entity = Pad:Wildcard(),
                Input = "Enable"
            }

            return timer
        end;
    }

end
```
