
## Basic Filters

Whenever Damage occurs:
```json
{
  "type": "DamageOccurs",
  "atLeast": 5
}
```

Whenever Target has at least health:
```json
{
  "type": "CombatantHasAtLeastHealth",
  "atLeast": 5,
  "combatant": "#target"
}
```

## Basic effects 

Deal damage to the target:
```json
{
  "type": "DamageCombatantEffect",
  "damage": 5,
  "combatant": "#target"
}
```


## full Whenever

Whenever Damage occurs, target takes 3 damage

```json
{
  "filters": [
    {
      "type": "DamageOccurs",
      "atLeast": 5
    }
  ],
  "effects": [
    {
      "type": "DamageCombatantEffect",
      "damage": 3,
      "combatant": "#target"
    }
  ]
}
```

Whenever Damage occurs and target is #cardCaster, target takes 3 healing

```json
{
  "filters": [
    {
      "type": "DamageOccurs",
      "atLeast": 1
    },
    {
      "type": "CombatantsAreSame",
      "variableAlias": "#target",
      "expectedAlias": "#cardCaster"
    }
  ],
  "effects": [
    {
      "type": "DamageCombatantEffect",
      "damage": -3,
      "combatant": "#target"
    }
  ]
}
```