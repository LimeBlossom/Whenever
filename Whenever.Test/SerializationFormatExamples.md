
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
  "type": "TargetHasAtLeastHealth",
  "atLeast": 5
}
```

## Basic effects 

Deal damage:
```json
{
  "type": "DealDamage",
  "amount": 5
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
      "type": "DealDamage",
      "amount": 3
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
      "type": "IdentityAliasMatches",
      "alias1": "#cardCaster",
      "alias2": "#currentTarget"
    }
  ],
  "effects": [
    {
      "type": "DealDamage",
      "amount": -3
    }
  ]
}
```