<template>
  <div class="box-banner">
    <div class="back"></div>

    <div class="scene">
      <div
        class="cube"
        v-bind:class="currentSideClass"
        v-on:click="onBoxClicked"
      >
        <div class="cubeFace cubeFaceRight">
          <div>{{ $t("main.banner1") }}</div>
        </div>
        <div class="cubeFace cubeFaceBack">
          <div>{{ $t("main.banner2") }}</div>
        </div>
        <div class="cubeFace cubeFaceLeft">
          <div>{{ $t("main.banner3") }}</div>
        </div>
        <div class="cubeFace cubeFaceFront">
          <div>{{ $t("main.banner4") }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "BoxBanner",
  data() {
    return {
      interval: null,
      currentSide: 1,
    };
  },
  mounted() {
    this.resetRotationInterval();
  },
  computed: {
    currentSideClass() {
      switch (this.currentSide) {
        case 0:
          return "show-front";
        case 1:
          return "show-right";
        case 2:
          return "show-back";
        default:
          return "show-left";
      }
    },
  },
  methods: {
    resetRotationInterval() {
      const rotationDurationMs = 10000;
      if (this.interval) {
        clearInterval(this.interval);
      }
      this.interval = setInterval(() => this.changeSide(), rotationDurationMs);
    },
    changeSide() {
      let newSide = (this.currentSide + 1) % 4;
      this.currentSide = newSide;
    },
    onBoxClicked() {
      this.changeSide();
      this.resetRotationInterval();
    },
  },
  beforeDestroy() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  },
};
</script>

<style scoped>
.box-banner {
  margin-top: 40px;
  position: relative;
}

.back {
  position: absolute;
  width: 100%;
  height: 100%;
  background: #692e08;
  clip-path: polygon(0 75%, 100% 0, 100% 100%, 0 100%);
}

.scene {
  margin: 0 auto;
  width: 500px;
  height: 500px;
  perspective: 600px;
}

.cube {
  width: 100%;
  height: 100%;
  position: relative;
  transform-style: preserve-3d;
  transition: transform 0.5s;
  cursor: pointer;
  user-select: none;
}

.cubeFace {
  position: absolute;
  width: 500px;
  height: 500px;
  background: url(../../assets/cardboard.png);
  background-size: 100%;
}

.cubeFace > div {
  padding: 25px;
  height: 100%;
  width: 100%;
  text-align: center;
  font-size: 3em;
  color: rgb(0, 0, 0);
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.cubeFaceFront {
  transform: rotateY(0deg) translateZ(250px);
}
.cubeFaceRight {
  transform: rotateY(90deg) translateZ(250px);
}
.cubeFaceBack {
  transform: rotateY(180deg) translateZ(250px);
}
.cubeFaceLeft {
  transform: rotateY(-90deg) translateZ(250px);
}

.cube.show-front {
  transform: translateZ(-300px) rotateY(-360deg);
}
.cube.show-right {
  transform: translateZ(-300px) rotateY(-90deg);
}
.cube.show-back {
  transform: translateZ(-300px) rotateY(-180deg);
}
.cube.show-left {
  transform: translateZ(-300px) rotateY(-270deg);
}

.cube.show-front .cubeFaceBack {
  display: none;
}
.cube.cube.show-left .cubeFaceRight {
  display: none;
}
.cube.show-back .cubeFaceFront {
  display: none;
}
</style>