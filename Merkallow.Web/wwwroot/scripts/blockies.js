﻿(function webpackUniversalModuleDefinition(root, factory) {
	if (typeof exports === 'object' && typeof module === 'object')
		module.exports = factory();
	else if (typeof define === 'function' && define.amd)
		define([], factory);
	else if (typeof exports === 'object')
		exports["ethereum-blockies-base64"] = factory();
	else
		root["ethereum-blockies-base64"] = factory();
})(this, function () {
	return /******/ (function (modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};

/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {

/******/ 		// Check if module is in cache
/******/ 		if (installedModules[moduleId])
/******/ 			return installedModules[moduleId].exports;

/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			exports: {},
/******/ 			id: moduleId,
/******/ 			loaded: false
				/******/
};

/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);

/******/ 		// Flag the module as loaded
/******/ 		module.loaded = true;

/******/ 		// Return the exports of the module
/******/ 		return module.exports;
			/******/
}


/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;

/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;

/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";

/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(0);
		/******/
})
/************************************************************************/
/******/([
/* 0 */
/***/ function (module, exports, __webpack_require__) {

			const pnglib = __webpack_require__(1);
			const hsl2rgb = __webpack_require__(2);

			// The random number is a js implementation of the Xorshift PRNG
			const randseed = new Array(4); // Xorshift: [x, y, z, w] 32 bit values

			function seedrand(seed) {
				for (let i = 0; i < randseed.length; i++) {
					randseed[i] = 0;
				}
				for (let i = 0; i < seed.length; i++) {
					randseed[i % 4] = (randseed[i % 4] << 5) - randseed[i % 4] + seed.charCodeAt(i);
				}
			}

			function rand() {
				// based on Java's String.hashCode(), expanded to 4 32bit values
				const t = randseed[0] ^ (randseed[0] << 11);

				randseed[0] = randseed[1];
				randseed[1] = randseed[2];
				randseed[2] = randseed[3];
				randseed[3] = randseed[3] ^ (randseed[3] >> 19) ^ t ^ (t >> 8);

				return (randseed[3] >>> 0) / (1 << 31 >>> 0);
			}

			function createColor() {
				//saturation is the whole color spectrum
				const h = Math.floor(rand() * 360);
				//saturation goes from 40 to 100, it avoids greyish colors
				const s = rand() * 60 + 40;
				//lightness can be anything from 0 to 100, but probabilities are a bell curve around 50%
				const l = (rand() + rand() + rand() + rand()) * 25;

				return [h / 360, s / 100, l / 100];
			}

			function createImageData(size) {
				const width = size; // Only support square icons for now
				const height = size;

				const dataWidth = Math.ceil(width / 2);
				const mirrorWidth = width - dataWidth;

				const data = [];
				for (let y = 0; y < height; y++) {
					let row = [];
					for (let x = 0; x < dataWidth; x++) {
						// this makes foreground and background color to have a 43% (1/2.3) probability
						// spot color has 13% chance
						row[x] = Math.floor(rand() * 2.3);
					}
					const r = row.slice(0, mirrorWidth).reverse();
					row = row.concat(r);

					for (let i = 0; i < row.length; i++) {
						data.push(row[i]);
					}
				}

				return data;
			}

			// Modifies the passed PNG to fill in a specified rectangle
			function fillRect(png, x, y, w, h, color) {
				for (let i = 0; i < w; i++) {
					for (let j = 0; j < h; j++) {
						png.buffer[png.index(x + i, y + j)] = color;
					}
				}
			}

			function buildOpts(opts) {
				if (!opts.seed) {
					throw new Error('No seed provided');
				}

				seedrand(opts.seed);

				return Object.assign({
					size: 8,
					scale: 16,
					color: createColor(),
					bgcolor: createColor(),
					spotcolor: createColor(),
				}, opts)
			}

			function makeBlockie(address) {
				const opts = buildOpts({ seed: address.toLowerCase() });

				const imageData = createImageData(opts.size);
				const width = Math.sqrt(imageData.length);

				const p = new pnglib(opts.size * opts.scale, opts.size * opts.scale, 3);
				const bgcolor = p.color(...hsl2rgb(...opts.bgcolor));
				const color = p.color(...hsl2rgb(...opts.color));
				const spotcolor = p.color(...hsl2rgb(...opts.spotcolor));

				for (let i = 0; i < imageData.length; i++) {
					const row = Math.floor(i / width);
					const col = i % width;
					// if data is 0, leave the background
					if (imageData[i]) {
						// if data is 2, choose spot color, if 1 choose foreground
						const pngColor = imageData[i] == 1 ? color : spotcolor;
						fillRect(p, col * opts.scale, row * opts.scale, opts.scale, opts.scale, pngColor);
					}
				}
				return `data:image/png;base64,${p.getBase64()}`;
			}

			module.exports = makeBlockie;


			/***/
},
/* 1 */
/***/ function (module, exports) {
			module.exports = function (width, height, depth) {


				// helper functions for that ctx
				function write(buffer, offs) {
					for (var i = 2; i < arguments.length; i++) {
						for (var j = 0; j < arguments[i].length; j++) {
							buffer[offs++] = arguments[i].charAt(j);
						}
					}
				}

				function byte2(w) {
					return String.fromCharCode((w >> 8) & 255, w & 255);
				}

				function byte4(w) {
					return String.fromCharCode((w >> 24) & 255, (w >> 16) & 255, (w >> 8) & 255, w & 255);
				}

				function byte2lsb(w) {
					return String.fromCharCode(w & 255, (w >> 8) & 255);
				}

				this.width = width;
				this.height = height;
				this.depth = depth;

				// pixel data and row filter identifier size
				this.pix_size = height * (width + 1);

				// deflate header, pix_size, block headers, adler32 checksum
				this.data_size = 2 + this.pix_size + 5 * Math.floor((0xfffe + this.pix_size) / 0xffff) + 4;

				// offsets and sizes of Png chunks
				this.ihdr_offs = 0; // IHDR offset and size
				this.ihdr_size = 4 + 4 + 13 + 4;
				this.plte_offs = this.ihdr_offs + this.ihdr_size; // PLTE offset and size
				this.plte_size = 4 + 4 + 3 * depth + 4;
				this.trns_offs = this.plte_offs + this.plte_size; // tRNS offset and size
				this.trns_size = 4 + 4 + depth + 4;
				this.idat_offs = this.trns_offs + this.trns_size; // IDAT offset and size
				this.idat_size = 4 + 4 + this.data_size + 4;
				this.iend_offs = this.idat_offs + this.idat_size; // IEND offset and size
				this.iend_size = 4 + 4 + 4;
				this.buffer_size = this.iend_offs + this.iend_size; // total PNG size

				this.buffer = new Array();
				this.palette = new Object();
				this.pindex = 0;

				var _crc32 = new Array();

				// initialize buffer with zero bytes
				for (var i = 0; i < this.buffer_size; i++) {
					this.buffer[i] = "\x00";
				}

				// initialize non-zero elements
				write(this.buffer, this.ihdr_offs, byte4(this.ihdr_size - 12), 'IHDR', byte4(width), byte4(height), "\x08\x03");
				write(this.buffer, this.plte_offs, byte4(this.plte_size - 12), 'PLTE');
				write(this.buffer, this.trns_offs, byte4(this.trns_size - 12), 'tRNS');
				write(this.buffer, this.idat_offs, byte4(this.idat_size - 12), 'IDAT');
				write(this.buffer, this.iend_offs, byte4(this.iend_size - 12), 'IEND');

				// initialize deflate header
				var header = ((8 + (7 << 4)) << 8) | (3 << 6);
				header += 31 - (header % 31);

				write(this.buffer, this.idat_offs + 8, byte2(header));

				// initialize deflate block headers
				for (var i = 0; (i << 16) - 1 < this.pix_size; i++) {
					var size, bits;
					if (i + 0xffff < this.pix_size) {
						size = 0xffff;
						bits = "\x00";
					} else {
						size = this.pix_size - (i << 16) - i;
						bits = "\x01";
					}
					write(this.buffer, this.idat_offs + 8 + 2 + (i << 16) + (i << 2), bits, byte2lsb(size), byte2lsb(~size));
				}

				/* Create crc32 lookup table */
				for (var i = 0; i < 256; i++) {
					var c = i;
					for (var j = 0; j < 8; j++) {
						if (c & 1) {
							c = -306674912 ^ ((c >> 1) & 0x7fffffff);
						} else {
							c = (c >> 1) & 0x7fffffff;
						}
					}
					_crc32[i] = c;
				}

				// compute the index into a png for a given pixel
				this.index = function (x, y) {
					var i = y * (this.width + 1) + x + 1;
					var j = this.idat_offs + 8 + 2 + 5 * Math.floor((i / 0xffff) + 1) + i;
					return j;
				}

				// convert a color and build up the palette
				this.color = function (red, green, blue, alpha) {

					alpha = alpha >= 0 ? alpha : 255;
					var color = (((((alpha << 8) | red) << 8) | green) << 8) | blue;

					if (typeof this.palette[color] == "undefined") {
						if (this.pindex == this.depth) return "\x00";

						var ndx = this.plte_offs + 8 + 3 * this.pindex;

						this.buffer[ndx + 0] = String.fromCharCode(red);
						this.buffer[ndx + 1] = String.fromCharCode(green);
						this.buffer[ndx + 2] = String.fromCharCode(blue);
						this.buffer[this.trns_offs + 8 + this.pindex] = String.fromCharCode(alpha);

						this.palette[color] = String.fromCharCode(this.pindex++);
					}
					return this.palette[color];
				}

				// output a PNG string, Base64 encoded
				this.getBase64 = function () {

					var s = this.getDump();

					var ch = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
					var c1, c2, c3, e1, e2, e3, e4;
					var l = s.length;
					var i = 0;
					var r = "";

					do {
						c1 = s.charCodeAt(i);
						e1 = c1 >> 2;
						c2 = s.charCodeAt(i + 1);
						e2 = ((c1 & 3) << 4) | (c2 >> 4);
						c3 = s.charCodeAt(i + 2);
						if (l < i + 2) { e3 = 64; } else { e3 = ((c2 & 0xf) << 2) | (c3 >> 6); }
						if (l < i + 3) { e4 = 64; } else { e4 = c3 & 0x3f; }
						r += ch.charAt(e1) + ch.charAt(e2) + ch.charAt(e3) + ch.charAt(e4);
					} while ((i += 3) < l);
					return r;
				}

				// output a PNG string
				this.getDump = function () {
					// compute adler32 of output pixels + row filter bytes
					var BASE = 65521; /* largest prime smaller than 65536 */
					var NMAX = 5552; /* NMAX is the largest n such that 255n(n+1)/2 + (n+1)(BASE-1) <= 2^32-1 */
					var s1 = 1;
					var s2 = 0;
					var n = NMAX;

					for (var y = 0; y < this.height; y++) {
						for (var x = -1; x < this.width; x++) {
							s1 += this.buffer[this.index(x, y)].charCodeAt(0);
							s2 += s1;
							if ((n -= 1) == 0) {
								s1 %= BASE;
								s2 %= BASE;
								n = NMAX;
							}
						}
					}
					s1 %= BASE;
					s2 %= BASE;
					write(this.buffer, this.idat_offs + this.idat_size - 8, byte4((s2 << 16) | s1));

					// compute crc32 of the PNG chunks
					function crc32(png, offs, size) {
						var crc = -1;
						for (var i = 4; i < size - 4; i += 1) {
							crc = _crc32[(crc ^ png[offs + i].charCodeAt(0)) & 0xff] ^ ((crc >> 8) & 0x00ffffff);
						}
						write(png, offs + size - 4, byte4(crc ^ -1));
					}

					crc32(this.buffer, this.ihdr_offs, this.ihdr_size);
					crc32(this.buffer, this.plte_offs, this.plte_size);
					crc32(this.buffer, this.trns_offs, this.trns_size);
					crc32(this.buffer, this.idat_offs, this.idat_size);
					crc32(this.buffer, this.iend_offs, this.iend_size);

					// convert PNG to string
					return "\x89PNG\r\n\x1a\n" + this.buffer.join('');
				}
			};


			/***/
},
/* 2 */
/***/ function (module, exports) {
			function hue2rgb(p, q, t) {
				if (t < 0) t += 1;
				if (t > 1) t -= 1;
				if (t < 1 / 6) return p + (q - p) * 6 * t;
				if (t < 1 / 2) return q;
				if (t < 2 / 3) return p + (q - p) * (2 / 3 - t) * 6;
				return p;
			}

			function hsl2rgb(h, s, l) {
				let r, g, b;

				if (s == 0) {
					r = g = b = l; // achromatic
				} else {
					const q = l < 0.5 ? l * (1 + s) : l + s - l * s;
					const p = 2 * l - q;
					r = hue2rgb(p, q, h + 1 / 3);
					g = hue2rgb(p, q, h);
					b = hue2rgb(p, q, h - 1 / 3);
				}

				return [Math.round(r * 255), Math.round(g * 255), Math.round(b * 255), 255];
			}

			module.exports = hsl2rgb;


			/***/
}
/******/])
});
;