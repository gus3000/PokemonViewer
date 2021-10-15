<template>
  <div id="app">
    <div v-for="pokemon in this.team" :key="pokemon.Nickname">
      <Pokemon v-bind="pokemon" :species-info="pokemonInfo[pokemon.Growth.Species]"/>
    </div>
<!--    <pre>{{ this.pokemonData }}</pre>-->
<!--    <pre>{{ this.pokemonInfo }}</pre>-->
  </div>
</template>

<script>
import Pokemon from './components/Pokemon.vue'
import Papaparse from 'papaparse'

export default {
  name: 'App',
  data() {
    return {
      pokemonData: {},
      pokemonInfo: {}
    }
  },
  created() {
    setInterval(() => {
      fetch('http://localhost:13000')
          .then(response => response.json())
          .then(data => this.pokemonData = data)
    }, 2000);

    fetch('/csv/pokemon_index_number.csv')
        .then(response => response.text())
        .then(text => Papaparse.parse(text))
        .then(array => {
          let infos = [];
          for (const info of array.data.slice(1)) {
            const index = parseInt(info[0]);
            const name = info[1];
            const type1 = info[2];
            const type2 = info[3];

            infos[index] = {
              name: name,
              type1: type1,
              type2: type2
            }
          }
          return infos;
        })
        .then(data => this.pokemonInfo = data);
  },
  computed: {
    team: function () {
      if (this.pokemonData.Team === undefined)
        return null;
      return this.pokemonData.Team.filter(pokemon => pokemon.OTId !== 0);
    }
  },
  components: {
    Pokemon
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  margin-top: 60px;
}
</style>
