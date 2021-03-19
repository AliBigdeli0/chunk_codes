let two;
let c_w_step;
let c_h_step;
let arr_number = new Array(9);
let selected_x = -100;
let selected_y = -100;

window.onload = () => {
    window.onkeydown = KeyDownSVG;

    var elem = document.getElementById("draw_shapes");
    const wi = window.innerWidth / 2;
    const he = window.innerHeight / 2;
    var params = { fullscreen: false, type: Two.Types.svg, autostart: true };
    two = new Two(params).appendTo(elem);
    two.renderer.setSize(wi, he);
    two.renderer.domElement.onclick = MouseDownSVG;

    two.scene.translation.set(two.width / 2, two.height / 2);

    c_w_step = wi / 9;
    c_h_step = he / 9;

    ///init array
    for (let i = 0; i < arr_number.length; i++) {
        arr_number[i] = new Array(9);
        for (let j = 0; j < arr_number[i].length; j++) {
            arr_number[i][j] = (-100);
        }
    }

    DrawNumbers();

    two.update();
};

function DrawNumbers() {
    two.clear();
    DrawBackground();

    ///init array
    for (let i = 0; i < arr_number.length; i++) {
        for (let j = 0; j < arr_number[i].length; j++) {
            if (arr_number[i][j] > 0)
                ShowBigNumbers(arr_number[i][j], i, j);
        }
    }
    two.render();
}

function DrawBackground() {
    two.scene.translation.set(0, 0);
    let index_c = 0;
    for (let index = 0; index <= two.width; index += c_w_step) {
        var line = two.makeLine(index, 0, index, two.height);
        line.stroke = "rgb(0,0,0)";
        if (index_c > 2 && (index_c % 3 == 0))
            line.linewidth = 3;
        index_c++;
    }

    index_c = 0;

    for (let index = 0; index <= two.height; index += c_h_step) {
        var line = two.makeLine(0, index, two.width, index);
        line.stroke = "rgb(0,0,0)";
        if (index_c > 2 && (index_c % 3 == 0))
            line.linewidth = 3;
        index_c++;
    }
}

function ShowBigNumbers(number, cell_x, cell_y) {
    if (number > 9 || number < 1) throw "error";
    let sx;
    let sy;
    sx = cell_x * c_w_step + c_w_step / 2;
    sy = cell_y * c_h_step + c_h_step / 2 + 3;

    var styles = {
        family: 'proxima-nova, sans-serif',
        size: 30,
        leading: 50,
        weight: 900
    };

    var directions = two.makeText(number, sx, sy, styles);
    directions.fill = 'black';
};

function ShowSmallNumber(number, cell_x, cell_y) {
    if (number > 9 || number < 1) throw "error";
    let sx;
    let sy;
    sx = cell_x * c_w_step;
    sy = cell_y * c_h_step;

    var styles = {
        family: 'proxima-nova, sans-serif',
        size: 10,
        leading: 50,
        weight: 100
    };

    let cell_inner_step_x = c_w_step / 4;
    let cell_inner_step_y = c_h_step / 4;

    if (number < 4) {
        sx = sx + cell_inner_step_x * number;
        sy = sy + cell_inner_step_y;
    } else if (number > 3 && number < 7) {
        sx = sx + cell_inner_step_x * (number - 3);
        sy = sy + cell_inner_step_y * 2;
    } else if (number > 6) {
        sx = sx + cell_inner_step_x * (number - 6);
        sy = sy + cell_inner_step_y * 3;
    }

    var directions = two.makeText(number, sx, sy, styles);
    directions.fill = 'black';
}

function clearValueClick() {
    for (let i = 0; i < arr_number.length; i++) {
        arr_number[i] = new Array(9);
        for (let j = 0; j < arr_number[i].length; j++) {
            arr_number[i][j] = -100;
        }
    }
    DrawNumbers();
}

function calculateClick() {
    //check inner cube
    for (let i = 0; i < arr_number.length; i++) {
        for (let j = 0; j < arr_number[i].length; j++) {
            let val = arr_number[i][j];
            if (val > 0) continue;

            var temp = new Array(9);
            temp[0] = (1);
            temp[1] = (2);
            temp[2] = (3);
            temp[3] = (4);
            temp[4] = (5);
            temp[5] = (6);
            temp[6] = (7);
            temp[7] = (8);
            temp[8] = (9);

            //rows
            for (let num = 0; num < arr_number.length; num++) {
                let remove_val = arr_number[num][j];
                RemoveArray(temp, remove_val);
            }

            //columns
            for (let num = 0; num < arr_number.length; num++) {
                let remove_val = arr_number[i][num];
                RemoveArray(temp, remove_val);
            }

            //inner
            let temp_i = i;
            let temp_j = j;

            if (temp_i > 2) {
                while (true) {
                    if (temp_i % 3 == 0)
                        break;
                    temp_i--;
                }
            } else {
                temp_i = 0;
            }

            if (temp_j > 2) {
                while (true) {
                    if (temp_j % 3 == 0)
                        break;
                    temp_j--;
                }
            } else {
                temp_j = 0;
            }

            for (let ii = temp_i; ii < temp_i + 3; ii++) {
                for (let jj = temp_j; jj < temp_j + 3; jj++) {
                    let elem = arr_number[ii][jj];
                    RemoveArray(temp, elem);
                } //jj
            } //ii

            // show elements
            for (let show = 0; show < temp.length; show++)
                ShowSmallNumber(temp[show], i, j);
        } //j 
    } // i

    two.update();
}

function RemoveArray(arr, number) {
    if (number < 1) return false;
    let index_remove = arr.indexOf(number)
    if (index_remove > -1) {
        arr.splice(index_remove, 1);
        return true;
    }
    return false;
}

function MouseDownSVG(e) {
    var x = parseInt((e.offsetX / two.width) * 9);
    var y = parseInt((e.offsetY / two.height) * 9);

    selected_x = x;
    selected_y = y;
    arr_number[selected_x][selected_y] = -100;
    DrawNumbers();
    let rect = two.makeRectangle(x * c_w_step + (c_w_step / 2), y * c_h_step + (c_h_step / 2), c_w_step, c_h_step);
    rect.fill = "rgba(0, 200, 255, 0.75)";
}

function KeyDownSVG(evt) {
    evt = evt || window.event;
    var charCode = evt.keyCode || evt.which;
    var charStr = String.fromCharCode(charCode);
    if (selected_x < 0 || selected_y < 0) return;
    arr_number[selected_x][selected_y] = parseInt(charStr);
    DrawNumbers();
}