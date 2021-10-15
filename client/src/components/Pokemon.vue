<template>
  <div class="pokebox" :style="style">
    <h4>{{ Nickname }}</h4>
    <!--    <pre>{{ this.speciesInfo }}</pre>-->
    <img class="sprite" v-bind:src="imgPath" :alt="speciesInfo.name">
    <b-progress class="hp" :max="TotalHp" variant="success" height="1rem">
      <b-progress-bar :value="CurrentHp" >
        <span>{{CurrentHp}}/{{TotalHp}}</span>

      </b-progress-bar>
    </b-progress>
  </div>
</template>

<script>
import typeColor from "@/helpers/colors";

export default {
  props: {
    speciesInfo: {
      name: String,
      type1: String,
      type2: String | null,
    },

    //serialized
    Misc: String,
    PersonalityValue: Number,
    OTId: Number,
    OTIdBinary: String,

    Nickname: String,

    //TODO language
    OTName: String,

    //TODO markings
    //TODO checksum
    Growth: {
      Species: Number,
      ItemHeld: Number,
      Experience: Number,
      PpBonuses: Number,
      Friendship: Number,
    },
    // Attacks: Attacks?,
    // EVsAndCondition: EVsAndCondition?,
    // Miscellaneous: Miscellaneous?,
    //TODO status
    Level: Number,

    //TODO pokerus
    CurrentHp: Number,
    TotalHp: Number,
    Attack: Number,
    Defense: Number,
    Speed: Number,
    SpecialAttack: Number,
    SpecialDefense: Number,
  },
  computed: {
    imgPath: function () {
      if (this.speciesInfo === undefined)
        return "";
      return require("..//assets/sprites/" + this.speciesInfo.name.toLowerCase() + ".gif");
    },
    background: function () {
      return "linear-gradient(90deg, rgb(132, 241, 0) 0%, rgb(0, 140, 255) 100%);";
    },
    typeColors: function () {
      return [
        typeColor.getTypeColor(this.speciesInfo.type1),
        typeColor.getTypeColor(this.speciesInfo.type2) ?? typeColor.getTypeColor(this.speciesInfo.type1)
      ]
    },
    style: function () {
      return "--type-1-color: " + this.typeColors[0] + ";"
          + "--type-2-color: " + this.typeColors[1] + ";"
    }
  }
}
</script>

<style scoped>
.pokebox {
  --type-1-color: white;
  --type-2-color: white;
  --border-color: lightgrey;
  --hp-color: green;
  max-width: 200px;
  text-align: center;
  background: linear-gradient(90deg, var(--type-1-color) 0%, var(--type-2-color) 100%);
  border-style: outset;
  border-color: var(--border-color);
  border-radius: 10px;
}

.sprite {
  display: block;
  margin-left: auto;
  margin-right: auto;
}

::-moz-progress-bar {
  background-color: var(--hp-color);
}

::-webkit-progress-value {
  background: var(--hp-color);
}

.hp {
  -webkit-appearance: none;
  background-color: white;
  height: 10px;
  border-radius: 5px;

  margin-left: auto;
  margin-right: auto;
//height: 10px; //background: black;
}
</style>